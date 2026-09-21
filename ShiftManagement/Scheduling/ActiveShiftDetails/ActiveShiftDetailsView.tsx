import { Guid } from '@cratis/fundamentals';
import { GetActiveShiftDetails } from './ActiveShiftDetails';

export const ActiveShiftDetailsView = ({ shiftId }: { shiftId: string }) => {
    const [result] = GetActiveShiftDetails.use({ shiftId: Guid.parse(shiftId) });
    const details = result.data;

    if (result.isPerforming) {
        return null;
    }

    return (
        <div className='p-4 border rounded mb-4'>
            <h2 className='text-xl font-semibold mb-2'>{details.name}</h2>
            <p><strong>Type:</strong> {details.type}</p>
            <p><strong>Recurring:</strong> {details.recurring ? 'Yes' : 'No'}</p>
            <p><strong>Days:</strong> {details.fromDay} – {details.toDay}</p>
            <p><strong>Start:</strong> {details.startDateTime?.toString()}</p>
            <p><strong>End:</strong> {details.endDateTime?.toString()}</p>
        </div>
    );
};
