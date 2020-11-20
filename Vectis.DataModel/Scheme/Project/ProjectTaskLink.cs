using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{

    /// <summary>
    /// A link between project tasks.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Project Task Link")]
    public class ProjectTaskLink : VectisBase
    {
        private LinkType linkType = LinkType.None;
        /// <summary>
        /// The link type. See <see cref="LinkType"/>.
        /// </summary>
        [MessagePack.Key(5)]
        public LinkType LinkType { get => linkType; set => Setter(ref linkType, value); }


        private string previousProjectTaskId;
        /// <summary>
        /// Id of the previous project task.
        /// </summary>
        [MessagePack.Key(6)]
        [CustomValidation(typeof(ProjectTaskLink), nameof(ValidatePreviousProjectTaskId))]
        public string PreviousProjectTaskId { get => previousProjectTaskId; set => Setter(ref previousProjectTaskId, value); }


        private int offsetMonths;
        /// <summary>
        /// An offset in months (positive means delay, negative advance) relative to the unadjusted linked dates.
        /// </summary>
        [MessagePack.Key(7)]
        [Display(Name = "Offset Period", Prompt = "Offset to the task's linked date in months")]
        [Required]
        public int OffsetMonths { get => offsetMonths; set => Setter(ref offsetMonths, value); }


        /// <summary>
        /// Returns a calculated start date for the relevant task.
        /// </summary>
        /// <param name="allTasks">A list of all tasks.</param>
        /// <param name="thisTask">The task for which the start date is required.</param>
        /// <returns></returns>
        public DateTime AssessedStartDate(GroupedDataset groupedDataset, ProjectTask thisTask)
        {
            var allTasks = groupedDataset?.GetItems<ProjectTask>();

            if (allTasks is null || allTasks.Count == 0)
            {
                return default;
            }

            if (LinkType == LinkType.None)
            {
                return thisTask.StartDate;
            }

            ProjectTask prevTask = groupedDataset.GetItem<ProjectTask>(PreviousProjectTaskId);
            DateTime startDate;

            if (LinkType == LinkType.StartToStart || LinkType == LinkType.StartToEnd)
            {
                startDate = prevTask.ActualStartDate;
            }
            else
            {
                startDate = prevTask.ActualEndDate;
            }

            if (LinkType == LinkType.StartToEnd || LinkType == LinkType.EndToEnd)
            {
                startDate = startDate.AddMonths(-thisTask.PeriodMonths);
            }

            startDate = startDate.AddMonths(OffsetMonths);

            return startDate;
        }


        /// <summary>
        /// Validates the previous project task.
        /// </summary>
        /// <param name="previousProjectTaskId"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public static ValidationResult ValidatePreviousProjectTaskId(string previousProjectTaskId, ValidationContext validationContext)
        {
            var thisLink = (ProjectTaskLink)validationContext.ObjectInstance;

            if (thisLink.LinkType == LinkType.None)
            {
                return ValidationResult.Success;
            }

            if (string.IsNullOrWhiteSpace(previousProjectTaskId))
            {
                return new ValidationResult("Cannot be empty");
            }

            try
            {
                _ = new Guid(previousProjectTaskId);
            }
            catch
            {
                return new ValidationResult("Must reference a valid precedent project task");
            }

            return ValidationResult.Success;
        }
    }
}
