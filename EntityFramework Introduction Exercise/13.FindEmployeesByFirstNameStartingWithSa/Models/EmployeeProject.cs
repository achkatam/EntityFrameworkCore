namespace SoftUni.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class EmployeeProject
    {
        [Key]
        [Column("EmployeeID")]
        public int EmployeeId { get; set; }
        [Key]
        [Column("ProjectID")]
        public int ProjectId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        [InverseProperty(nameof(Models.Employee.EmployeesProjects))]
        public virtual Employee Employee { get; set; }
        [ForeignKey(nameof(ProjectId))]
        [InverseProperty(nameof(Models.Project.EmployeesProjects))]
        public virtual Project Project { get; set; }
    }
}
