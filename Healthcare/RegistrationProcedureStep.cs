﻿using System;
using System.Collections.Generic;
using System.Xml;
using ClearCanvas.Common;

namespace ClearCanvas.Healthcare
{
	[ExtensionOf(typeof(ProcedureStepBuilderExtensionPoint))]
	public class RegistrationProcedureStepBuilder : ProcedureStepBuilderBase
	{

		public override Type ProcedureStepClass
		{
			get { return typeof(RegistrationProcedureStep); }
		}

		public override ProcedureStep CreateInstance(XmlElement xmlNode, Procedure procedure)
		{
			return new RegistrationProcedureStep();
		}

		public override void SaveInstance(ProcedureStep prototype, XmlElement xmlNode)
		{
		}
	}

	public class RegistrationProcedureStep : ProcedureStep
	{
		public RegistrationProcedureStep(Procedure procedure)
			: base(procedure)
		{
		}

		/// <summary>
		/// Default no-args constructor required by NHibernate
		/// </summary>
		public RegistrationProcedureStep()
		{
		}

		public override string Name
		{
			get { return "Registration"; }
		}

		public override bool IsPreStep
		{
			get { return true; }
		}

		public override List<Procedure> GetLinkedProcedures()
		{
			return new List<Procedure>();
		}

		protected override ProcedureStep CreateScheduledCopy()
		{
			return new RegistrationProcedureStep(this.Procedure);
		}

		protected override bool IsRelatedStep(ProcedureStep step)
		{
			// registration steps do not have related steps
			return false;
		}
	}
}
