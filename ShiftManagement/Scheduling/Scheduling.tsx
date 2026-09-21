import { Button } from 'primereact/button';
import { CreateShiftDialog } from './CreateShift';
import { useDialog } from '@cratis/arc.react/dialogs';

export const Scheduling = () => {
    const [CreateShift, showCreateShift] = useDialog(CreateShiftDialog);

    return (
        <div className='p-4'>
            <div className='flex justify-between items-center mb-4'>
                <h1 className='text-2xl font-bold'>Shift Scheduling</h1>
                <Button
                    label='Create Shift'
                    icon='pi pi-plus'
                    onClick={() => showCreateShift()} />
            </div>
            <CreateShift />
        </div>
    );
};
