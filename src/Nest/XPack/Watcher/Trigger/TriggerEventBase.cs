﻿using System.Runtime.Serialization;

namespace Nest
{
	[DataContract]
	public interface ITriggerEvent { }

	public abstract class TriggerEventBase : ITriggerEvent
	{
		public static implicit operator TriggerEventContainer(TriggerEventBase trigger) => trigger == null
			? null
			: new TriggerEventContainer(trigger);

		internal abstract void WrapInContainer(ITriggerEventContainer container);
	}
}
