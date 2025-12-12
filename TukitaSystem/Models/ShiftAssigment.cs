using System;

namespace TukitaSystem
{
    public class ShiftAssignment
    {
        public DateTime Date { get; private set; }
        public bool IsPresent { get; private set; }
        public double HoursWorked { get; private set; }

        public Employee Employee { get; private set; }
        public Shift Shift { get; private set; }

        public ShiftAssignment(Employee employee, Shift shift, DateTime date, bool isPresent, double hoursWorked)
        {
            if (hoursWorked < 0)
                throw new ArgumentException("Hours worked must be >= 0");
            
            if (employee.Assignments.Any(a => a.Shift == shift) ||
                shift.Assignments.Any(a => a.Employee == employee))
            {
                throw new InvalidOperationException("An assignment for this employee or shift already exists");
            }

            Employee = employee;
            Shift = shift;
            Date = date;
            IsPresent = isPresent;
            HoursWorked = hoursWorked;
            
            employee.AddAssignment(this);
            shift.AddAssignment(this);
        }

        public void UpdateAttendance(bool isPresent, double hoursWorked)
        {
            if (hoursWorked < 0)
                throw new ArgumentException("Hours worked must be >= 0.");

            IsPresent = isPresent;
            HoursWorked = hoursWorked;
        }
        
        public void Remove()
        {
            if (Shift.Assignments.Count == 1)
                throw new InvalidOperationException(
                    "Cannot remove the last ShiftAssignment from a shift");
            
            Employee.RemoveAssignment(this);
            Shift.RemoveAssignment(this);

            Employee = null!;
            Shift = null!;
        }
    }
}