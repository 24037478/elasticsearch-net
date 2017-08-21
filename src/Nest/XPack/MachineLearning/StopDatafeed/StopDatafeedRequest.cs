using System;
using Newtonsoft.Json;

namespace Nest
{
	public partial interface IStopDatafeedRequest
	{
		[JsonProperty("timeout")]
		Time Timeout { get; set; }

		[JsonProperty("force")]
		bool Force { get; set; }
	}

	/// <inheritdoc />
	public partial class StopDatafeedRequest
	{
		/// <inheritdoc />
		public Time Timeout { get; set; }

		/// <inheritdoc />
		public bool Force { get; set; }
	}

	/// <inheritdoc />
	[DescriptorFor("XpackMlStopDatafeed")]
	public partial class StopDatafeedDescriptor
	{
		Time IStopDatafeedRequest.Timeout { get; set; }
		bool IStopDatafeedRequest.Force { get; set; }

		/// <inheritdoc />
		public StopDatafeedDescriptor Timeout(Time timeout) => Assign(a => a.Timeout = timeout);

		/// <inheritdoc />
		public StopDatafeedDescriptor Force(bool force) => Assign(a => a.Force = force);
	}
}
