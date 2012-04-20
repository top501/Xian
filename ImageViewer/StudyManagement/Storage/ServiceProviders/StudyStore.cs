﻿using System;
using ClearCanvas.Common;
using ClearCanvas.ImageViewer.Common.StudyManagement;

namespace ClearCanvas.ImageViewer.StudyManagement.Storage.ServiceProviders
{
    [ExtensionOf(typeof(ServiceProviderExtensionPoint))]
    internal class StudyStoreQueryServiceProvider : IServiceProvider
    {
        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            if (serviceType != typeof(IStudyStoreQuery))
                return null;

            return new StudyStoreQueryProxy();
        }

        #endregion
    }

    internal class StudyStoreQueryProxy : IStudyStoreQuery
    {
        private GetStudyCountResult GetStudyCount(GetStudyCountRequest request)
        {
            using (var context = new DataAccessContext())
            {
                var count = context.GetStudyStoreQuery().GetStudyCount(request.Criteria);
                return new GetStudyCountResult { StudyCount = count };
            }
        }

        private GetStudyEntriesResult GetStudyEntries(GetStudyEntriesRequest request)
        {
            using (var context = new DataAccessContext())
            {
                var entries = context.GetStudyStoreQuery().GetStudyEntries(request.Criteria);
                return new GetStudyEntriesResult {StudyEntries = entries};
            }
        }

        private GetSeriesEntriesResult GetSeriesEntries(GetSeriesEntriesRequest request)
        {
            using (var context = new DataAccessContext())
            {
                var entries = context.GetStudyStoreQuery().GetSeriesEntries(request.Criteria);
                return new GetSeriesEntriesResult { SeriesEntries = entries };
            }
        }

        private GetImageEntriesResult GetImageEntries(GetImageEntriesRequest request)
        {
            using (var context = new DataAccessContext())
            {
                var entries = context.GetStudyStoreQuery().GetImageEntries(request.Criteria);
                return new GetImageEntriesResult { ImageEntries = entries };
            }
        }

        #region IStudyStoreQuery Members

        GetStudyCountResult IStudyStoreQuery.GetStudyCount(GetStudyCountRequest request)
        {
            return ServiceProxyHelper.Call(GetStudyCount, request);
        }

        GetStudyEntriesResult IStudyStoreQuery.GetStudyEntries(GetStudyEntriesRequest request)
        {
            return ServiceProxyHelper.Call(GetStudyEntries, request);
        }

        GetSeriesEntriesResult IStudyStoreQuery.GetSeriesEntries(GetSeriesEntriesRequest request)
        {
            return ServiceProxyHelper.Call(GetSeriesEntries, request);
        }

        GetImageEntriesResult IStudyStoreQuery.GetImageEntries(GetImageEntriesRequest request)
        {
            return ServiceProxyHelper.Call(GetImageEntries, request);
        }

        #endregion
    }
}
