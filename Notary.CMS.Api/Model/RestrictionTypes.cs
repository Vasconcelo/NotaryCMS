namespace Notary.CMS.Api.Model
{
    using System;

    public class RestrictionTypes
    {
        public Guid OrganizationRestrictionType { get; set; }

        public Guid StateRestrictionType { get; set; }

        public Guid MemberTransactionType { get; set; }

        public Guid PackageType { get; set; }

        public Guid PackageCategory { get; set; }
    }
}
