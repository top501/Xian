using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using ClearCanvas.Common;

namespace ClearCanvas.ImageViewer.Thumbnails
{
    public class LoadThumbnailRequest : IEquatable<LoadThumbnailRequest>
    {
        public LoadThumbnailRequest(ThumbnailDescriptor descriptor, Size size, Action<LoadThumbnailResult> resultCallback)
        {
            Platform.CheckForNullReference(descriptor, "descriptor");
            Platform.CheckForNullReference(resultCallback, "resultCallback");

            Descriptor = descriptor;
            Size = size;
            ResultCallback = resultCallback;
        }

        public readonly ThumbnailDescriptor Descriptor;
        public readonly Size Size;
        public readonly Action<LoadThumbnailResult> ResultCallback;

        public override string ToString()
        {
            return String.Format("{0}/{1}x{2}", Descriptor, Size.Width, Size.Height);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is LoadThumbnailRequest)
                return Equals((LoadThumbnailRequest) obj);

            return false;
        }

        #region IEquatable<LoadThumbnailRequest> Members

        public bool Equals(LoadThumbnailRequest other)
        {
            return other != null && Equals(other.Descriptor, Descriptor) && other.Size.Equals(Size);
        }

        #endregion
    }

    public class LoadThumbnailResult
    {
        public LoadThumbnailResult(ThumbnailDescriptor descriptor, IThumbnailData thumbnailData)
        {
            Platform.CheckForNullReference(descriptor, "descriptor");
            Platform.CheckForNullReference(thumbnailData, "thumbnailData");

            Descriptor = descriptor;
            ThumbnailData = thumbnailData;
        }

        public LoadThumbnailResult(ThumbnailDescriptor descriptor, Exception error)
        {
            Platform.CheckForNullReference(descriptor, "descriptor");
            Platform.CheckForNullReference(error, "error");

            Descriptor = descriptor;
            Error = error;
        }

        public readonly ThumbnailDescriptor Descriptor;
        public readonly IThumbnailData ThumbnailData;
        public readonly Exception Error;
    }

    public interface IThumbnailLoader
    {
        IThumbnailData GetDummyThumbnail(string message, Size size);
        bool TryGetThumbnail(ThumbnailDescriptor descriptor, Size size, out IThumbnailData thumbnail);

        void LoadThumbnailAsync(LoadThumbnailRequest request);
        void Cancel(LoadThumbnailRequest request);
        void Cancel(IEnumerable<LoadThumbnailRequest> requests);
    }

    public class ThumbnailLoader : IThumbnailLoader
    {
        private readonly IThumbnailRepository _repository;
        private readonly object _syncLock = new object();
        private readonly List<LoadThumbnailRequest> _pendingRequests = new List<LoadThumbnailRequest>();
        private bool _isLoading;

        public ThumbnailLoader()
            : this(ThumbnailRepository.Create())
        {
        }

        public ThumbnailLoader(IThumbnailRepository repository)
        {
            _repository = repository;
        }

        #region IThumbnailLoader Members

        public IThumbnailData GetDummyThumbnail(string message, Size size)
        {
            return _repository.GetDummyThumbnail(message, size);
        }

        public bool TryGetThumbnail(ThumbnailDescriptor descriptor, Size size, out IThumbnailData bitmap)
        {
            return _repository.TryGetThumbnail(descriptor, size, out bitmap);
        }

        public void LoadThumbnailAsync(LoadThumbnailRequest request)
        {
            lock (_syncLock)
            {
                _pendingRequests.Add(request);
                if (_isLoading)
                    return;

                _isLoading = true;
                ThreadPool.QueueUserWorkItem(Load, null);
            }
        }

        public void Cancel(LoadThumbnailRequest request)
        {
            lock(_syncLock)
            {
                _pendingRequests.Remove(request);
            }
        }

        public void Cancel(IEnumerable<LoadThumbnailRequest> requests)
        {
            lock (_syncLock)
            {
                foreach(var request in requests)
                _pendingRequests.Remove(request);
            }
        }

        #endregion

        public void Load(object state)
        {
            while (true)
            {
                LoadThumbnailRequest request;
                lock (_syncLock)
                {
                    if (_pendingRequests.Count == 0)
                    {
                        _isLoading = false;
                        break;
                    }

                    request = _pendingRequests[0];
                    _pendingRequests.RemoveAt(0);
                }

                LoadThumbnailResult result;

                try
                {
                    var image = _repository.GetThumbnail(request.Descriptor, request.Size);
                    result = new LoadThumbnailResult(request.Descriptor, image);
                }
                catch (Exception e)
                {
                    result = new LoadThumbnailResult(request.Descriptor, e);
                }

                request.ResultCallback(result);
            }
        }
    }
}