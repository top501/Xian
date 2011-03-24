﻿#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Collections.Generic;
using System.Reflection;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Dicom;

namespace ClearCanvas.ImageServer.Model
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
    public class DicomFieldContainerAttribute:Attribute
    {
        
    }
    public static class DataLengthValidation
    {
        public const uint UnknownLength = uint.MaxValue;

        private static readonly Dictionary<uint, uint> _maxLengths = new Dictionary<uint, uint>();

        static DataLengthValidation()
        {
            // Perhaps this should be taken from the database instead
            AddDicomLength(DicomTags.AccessionNumber);
            AddDicomLength(DicomTags.PatientId);
            AddDicomLength(DicomTags.IssuerOfPatientId);
            AddDicomLength(DicomTags.PatientsName);
            AddDicomLength(DicomTags.PatientsSex);
            AddDicomLength(DicomTags.PatientsBirthDate);
            AddDicomLength(DicomTags.SeriesDescription);
            AddDicomLength(DicomTags.SeriesNumber);
            AddDicomLength(DicomTags.Modality);
            AddDicomLength(DicomTags.SourceApplicationEntityTitle);
        }

        static void AddDicomLength(uint tag)
        {
            _maxLengths.Add(tag, DicomTagDictionary.GetDicomTag(tag).VR.MaximumLength);
        }

        public static uint GetMaxLength(uint tag)
        {
            uint length;
            if (_maxLengths.TryGetValue(tag, out length))
                return length;

            return UnknownLength;
        }

        public static bool IsExceedingMaxLength(uint tag, string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            return value.Length > GetMaxLength(tag);
        }

        /// <summary>
        /// Helper method to find properties in an object that are decorated with <see cref="DicomFieldAttribute"/> and 
        /// whose values exceed the max length allowed
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static List<uint> CheckDataLength(object entity)
        {
            List<uint> offendingTags = new List<uint>();

            PropertyInfo[] properties =
                entity.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public |
                                               BindingFlags.FlattenHierarchy);

            foreach(PropertyInfo p in properties)
            {
                //Nested property
                if (!AttributeUtils.HasAttribute<DicomFieldAttribute>(p))
                {
                    if (AttributeUtils.HasAttribute<DicomFieldContainerAttribute>(p) ||
                        AttributeUtils.HasAttribute < DicomFieldContainerAttribute>(p.DeclaringType))
                    {
                        offendingTags.AddRange(CheckDataLength(p.GetValue(entity, null)));
                    }
                }
                else
                {
                    var attr = AttributeUtils.GetAttribute<DicomFieldAttribute>(p);

                    var value = p.GetValue(entity, null) as string;
                    if (IsExceedingMaxLength(attr.Tag.TagValue, value))
                    {
                        if (!offendingTags.Contains(attr.Tag.TagValue))
                            offendingTags.Add(attr.Tag.TagValue);
                    }
                }
            }

            return offendingTags;
        }
    }
}
