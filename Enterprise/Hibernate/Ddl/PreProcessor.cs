﻿#region License

// Copyright (c) 2010, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Common;
using NHibernate.Cfg;

namespace ClearCanvas.Enterprise.Hibernate.Ddl
{
    /// <summary>
    /// Defines an extension point for pre-processing the NHibernate configuration prior to generating DDL scripts.
    /// </summary>
    [ExtensionPoint]
    public class DdlPreProcessorExtensionPoint : ExtensionPoint<IDdlPreProcessor>
    {
    }

	/// <summary>
	/// Pre-processes the NHibernate configuration prior to generating output.
	/// </summary>
    public class PreProcessor
    {
        private readonly bool _createIndexes;
        private readonly bool _autoIndexForeignKeys;


		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="createIndexes"></param>
		/// <param name="autoIndexForeignKeys"></param>
        public PreProcessor(bool createIndexes, bool autoIndexForeignKeys)
        {
            _createIndexes = createIndexes;
            _autoIndexForeignKeys = autoIndexForeignKeys;
        }

		/// <summary>
		/// Processes the specified configuration.
		/// </summary>
		/// <param name="store"></param>
		public void Process(PersistentStore store)
        {
			var config = store.Configuration;

            // order is important

            // run the enum FK processor first
			new EnumForeignKeyProcessor().Process(config);

            if(_createIndexes)
            {
                if (_autoIndexForeignKeys)
                {
                    // run the fk index creator
					new ForeignKeyIndexProcessor().Process(config);
                }

                // run the additional index creator
				new AdditionalIndexProcessor().Process(config);
            }

            // run extension processors
            foreach (IDdlPreProcessor processor in new DdlPreProcessorExtensionPoint().CreateExtensions())
            {
				processor.Process(config);
            }
        }
    }
}
