import { Guid } from '@cratis/fundamentals';
import { GetShiftAssignments } from './ShiftAssignments';

export const ShiftAssignmentsView = ({ shiftId }: { shiftId: string }) => {
    const [result] = GetShiftAssignments.use({ shiftId: Guid.parse(shiftId) });
    const assignments = result.data;

    if (result.isPerforming) {
        return null;
    }

    return (
        <div className='p-4 border rounded mb-4'>
            <h3 className='text-lg font-semibold mb-2'>Assigned Employees</h3>
            {assignments.employeeIds.length === 0 ? (
                <p>No employees assigned yet.</p>
            ) : (
                <ul>
                    {assignments.employeeIds.map(employeeId => (
                        <li key={employeeId.toString()}>{employeeId.toString()}</li>
                    ))}
                </ul>
            )}
        </div>
    );
};
