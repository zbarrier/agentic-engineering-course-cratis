import { Button } from 'primereact/button';
import { useParams } from 'react-router-dom';
import { CreateShiftDialog } from './CreateShift';
import { AssignEmployeeToShiftDialog } from './AssignEmployeeToShift';
import { ActiveShiftDetailsView } from './ActiveShiftDetails';
import { useDialog } from '@cratis/arc.react/dialogs';

export const Scheduling = () => {
    const { shiftId } = useParams<{ shiftId?: string }>();
    const [CreateShift, showCreateShift] = useDialog(CreateShiftDialog);
    const [AssignEmployeeToShift, showAssignEmployeeToShift] = useDialog(AssignEmployeeToShiftDialog);

    return (
        <div className='p-4'>
            <div className='flex justify-between items-center mb-4'>
                <h1 className='text-2xl font-bold'>Shift Scheduling</h1>
                {shiftId ? (
                    <Button
                        label='Assign Employee'
                        icon='pi pi-user-plus'
                        onClick={() => showAssignEmployeeToShift()} />
                ) : (
                    <Button
                        label='Create Shift'
                        icon='pi pi-plus'
                        onClick={() => showCreateShift()} />
                )}
            </div>
            {shiftId && <ActiveShiftDetailsView shiftId={shiftId} />}
            <CreateShift />
            <AssignEmployeeToShift />
        </div>
    );
};
