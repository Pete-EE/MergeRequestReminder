using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MergeRequestReminder.Model;

public class Approval
{
        [JsonProperty("id")] 
        public int Id { get; set; }

        [JsonProperty("iid")] 
        public int Iid { get; set; }

        [JsonProperty("project_id")] 
        public int ProjectId { get; set; }

        [JsonProperty("title")] 
        public string Title { get; set; }

        [JsonProperty("description")] 
        public string Description { get; set; }

        [JsonProperty("state")] 
        public string State { get; set; }

        [JsonProperty("created_at")] 
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")] 
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("merge_status")] 
        public string MergeStatus { get; set; }

        [JsonProperty("approved")] 
        public bool Approved { get; set; }

        [JsonProperty("approvals_required")] 
        public int ApprovalsRequired { get; set; }

        [JsonProperty("approvals_left")] 
        public int ApprovalsLeft { get; set; }

        [JsonProperty("require_password_to_approve")]
        public bool RequirePasswordToApprove { get; set; }

        [JsonProperty("approved_by")] 
        public List<ApprovedByUser> ApprovedBy { get; set; }

        [JsonProperty("suggested_approvers")] 
        public List<GitlabUser> SuggestedApprovers { get; set; }

        [JsonProperty("approvers")] 
        public List<GitlabUser> Approvers { get; set; }

        [JsonProperty("approver_groups")] 
        public List<object> ApproverGroups { get; set; }

        [JsonProperty("user_has_approved")] 
        public bool UserHasApproved { get; set; }

        [JsonProperty("user_can_approve")] 
        public bool UserCanApprove { get; set; }

        [JsonProperty("approval_rules_left")] 
        public List<object> ApprovalRulesLeft { get; set; }

        [JsonProperty("has_approval_rules")] 
        public bool HasApprovalRules { get; set; }

        [JsonProperty("merge_request_approvers_available")]
        public bool MergeRequestApproversAvailable { get; set; }

        [JsonProperty("multiple_approval_rules_available")]
        public bool MultipleApprovalRulesAvailable { get; set; }
}