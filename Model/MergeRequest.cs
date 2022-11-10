using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MergeRequestReminder.Model;

public class MergeRequest
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
    public DateTime? CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [JsonProperty("merged_by")]
    public GitlabUser MergedBy { get; set; }

    [JsonProperty("merge_user")]
    public GitlabUser MergeUser { get; set; }

    [JsonProperty("merged_at")]
    public DateTime? MergedAt { get; set; }

    [JsonProperty("closed_by")]
    public object ClosedBy { get; set; }

    [JsonProperty("closed_at")]
    public DateTime? ClosedAt { get; set; }

    [JsonProperty("target_branch")]
    public string TargetBranch { get; set; }

    [JsonProperty("source_branch")]
    public string SourceBranch { get; set; }

    [JsonProperty("user_notes_count")]
    public int UserNotesCount { get; set; }

    [JsonProperty("upvotes")]
    public int Upvotes { get; set; }

    [JsonProperty("downvotes")]
    public int Downvotes { get; set; }

    [JsonProperty("author")]
    public GitlabUser Author { get; set; }

    [JsonProperty("assignees")]
    public List<object> Assignees { get; set; }

    [JsonProperty("assignee")]
    public GitlabUser Assignee { get; set; }

    [JsonProperty("reviewers")]
    public List<GitlabUser> Reviewers { get; set; }

    [JsonProperty("source_project_id")]
    public int SourceProjectId { get; set; }

    [JsonProperty("target_project_id")]
    public int TargetProjectId { get; set; }

    [JsonProperty("labels")]
    public List<object> Labels { get; set; }

    [JsonProperty("draft")]
    public bool Draft { get; set; }

    [JsonProperty("work_in_progress")]
    public bool WorkInProgress { get; set; }

    [JsonProperty("milestone")]
    public object Milestone { get; set; }

    [JsonProperty("merge_when_pipeline_succeeds")]
    public bool MergeWhenPipelineSucceeds { get; set; }

    [JsonProperty("merge_status")]
    public string MergeStatus { get; set; }

    [JsonProperty("sha")]
    public string Sha { get; set; }

    [JsonProperty("merge_commit_sha")]
    public string MergeCommitSha { get; set; }

    [JsonProperty("squash_commit_sha")]
    public object SquashCommitSha { get; set; }

    [JsonProperty("discussion_locked")]
    public object DiscussionLocked { get; set; }

    [JsonProperty("should_remove_source_branch")]
    public bool? ShouldRemoveSourceBranch { get; set; }

    [JsonProperty("force_remove_source_branch")]
    public bool? ForceRemoveSourceBranch { get; set; }

    [JsonProperty("reference")]
    public string Reference { get; set; }
        
    [JsonProperty("references")]
    public References References { get; set; }

    [JsonProperty("web_url")]
    public string WebUrl { get; set; }

    [JsonProperty("time_stats")]
    public TimeStats TimeStats { get; set; }

    [JsonProperty("squash")]
    public bool Squash { get; set; }

    [JsonProperty("task_completion_status")]
    public TaskCompletionStatus TaskCompletionStatus { get; set; }

    [JsonProperty("has_conflicts")]
    public bool HasConflicts { get; set; }

    [JsonProperty("blocking_discussions_resolved")]
    public bool BlockingDiscussionsResolved { get; set; }

    [JsonProperty("approvals_before_merge")]
    public object ApprovalsBeforeMerge { get; set; }
}