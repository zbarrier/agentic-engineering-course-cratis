import { useState } from 'react';
import { useParams } from 'react-router-dom';
import { CommandDialog } from '@cratis/components/CommandDialog';
import { InputText } from 'primereact/inputtext';
import { Guid } from '@cratis/fundamentals';
import { AssignEmployeeToShift } from './AssignEmployeeToShift';

export const AssignEmployeeToShiftDialog = () => {
    const { shiftId } = useParams<{ shiftId: string }>();
    const [employeeId, setEmployeeId] = useState('');

    return (
        <CommandDialog
            command={AssignEmployeeToShift}
            title='Assign Employee to Shift'
            okLabel='Assign'
            cancelLabel='Cancel'
            onBeforeExecute={command => {
                command.shiftId = Guid.parse(shiftId!);
                command.employeeId = Guid.parse(employeeId);
                return command;
            }}>
            <div className='field'>
                <label htmlFor='employeeId'>Employee ID</label>
                <InputText
                    id='employeeId'
                    className='w-full'
                    value={employeeId}
                    onChange={event => setEmployeeId(event.target.value)} />
            </div>
        </CommandDialog>
    );
};
