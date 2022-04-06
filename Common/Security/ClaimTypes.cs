﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Common.Security
{
	/// <summary>
	/// Provides a list of NNA claim types.
	/// </summary>
    public static partial class ClaimTypes
    {
		/// <summary>
		/// Enables administrators to manage access to roles.
		/// </summary>
        public const string Role = @"urn:nna:security:claims:201502:role";

		/// <summary>
		/// Enables administrators to manage access to operations or methods.
		/// </summary>
        public const string Operation = @"urn:nna:security:claims:201502:operation";

		/// <summary>
		/// Assigned to users to define the scope of data that they have access to.
		/// </summary>
        public const string Scope = @"urn:nna:security:claims:201502:scope";

		/// <summary>
		/// The user type: Application, Person, etc.
		/// </summary>
        public const string UserType = @"urn:nna:security:claims:201502:usertype";

		/// <summary>
		/// The unique id that is associated with the user.
		/// </summary>
        public const string UserId = @"urn:nna:security:claims:201502:userid";

		/// <summary>
		/// If the use logon is PCI compliant, then True, otherwise False.
		/// </summary>
        public const string PCI = @"urn:nna:security:claims:201502:pci";

		/// <summary>
		/// The user id type: OAuthAccountGuid.
		/// </summary>
        public const string UserIdType = @"urn:nna:security:claims:201502:useridtype";

		/// <summary>
		/// The application specific user id.
		/// </summary>
        public const string UserAppId = @"urn:nna:security:claims:201502:userappid";

		/// <summary>
		/// The unique identifier for an application.
		/// </summary>
        public const string AppId = @"urn:nna:security:claims:201502:appid";

		/// <summary>
		/// The URI for an application.
		/// </summary>
        public const string AppUri = @"urn:nna:security:claims:201502:appuri";

		/// <summary>
		/// The unique id that is associated with the user's organization.
		/// </summary>
        public const string OrganizationId = @"urn:nna:security:claims:201502:organizationid";

		/// <summary>
		/// The id that identifies a service request message.
		/// </summary>
        public const string MessageId = @"urn:nna:security:claims:201502:messageid";

	}
}