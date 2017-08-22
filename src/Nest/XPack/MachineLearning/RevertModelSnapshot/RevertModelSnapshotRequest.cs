using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using Elasticsearch.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nest
{
	/// <summary>
	/// A request to revert a specific snapshot for a Machine Learning job
	/// </summary>
	public partial interface IRevertModelSnapshotRequest
	{
		/// <summary>
		/// If true, deletes the results in the time period between the latest results and the time of
		/// the reverted snapshot.
		/// It also resets the model to accept records for this time period. The default value is false.
		/// </summary>
		[JsonProperty("delete_intervening_results")]
		bool? DeleteInterveningResults { get; set; }
	}

	/// <inheritdoc />
	public partial class RevertModelSnapshotRequest
	{
		/// <inheritdoc />
		public bool? DeleteInterveningResults { get; set; }
	}

	/// <inheritdoc />
	[DescriptorFor("XpackMlRevertModelSnapshot")]
	public partial class RevertModelSnapshotDescriptor
	{
		bool? IRevertModelSnapshotRequest.DeleteInterveningResults { get; set; }

		/// <inheritdoc />
		public RevertModelSnapshotDescriptor DeleteInterveningResults(bool deleteInterveningResults = true) =>
			Assign(a => a.DeleteInterveningResults = deleteInterveningResults);
	}
}
