import { CommandDialog } from '@cratis/components/CommandDialog';
import { InputTextField, CheckboxField, DropdownField, CalendarField } from '@cratis/components/CommandForm';
import { CreateShift } from './CreateShift';

const days = [
    { label: 'Monday', value: 'Monday' },
    { label: 'Tuesday', value: 'Tuesday' },
    { label: 'Wednesday', value: 'Wednesday' },
    { label: 'Thursday', value: 'Thursday' },
    { label: 'Friday', value: 'Friday' },
    { label: 'Saturday', value: 'Saturday' },
    { label: 'Sunday', value: 'Sunday' },
];

export const CreateShiftDialog = () => (
    <CommandDialog
        command={CreateShift}
        title='Create Shift'
        okLabel='Create Shift'
        cancelLabel='Cancel'>
        <InputTextField<CreateShift>
            value={c => c.name}
            title='Name' />
        <InputTextField<CreateShift>
            value={c => c.type}
            title='Type' />
        <CheckboxField<CreateShift>
            value={c => c.recurring}
            label='Recurring' />
        <DropdownField<CreateShift>
            value={c => c.fromDay}
            title='From Day'
            options={days}
            optionLabel='label'
            optionValue='value' />
        <DropdownField<CreateShift>
            value={c => c.toDay}
            title='To Day'
            options={days}
            optionLabel='label'
            optionValue='value' />
        <CalendarField<CreateShift>
            value={c => c.startDateTime}
            title='Start'
            showTime
            showIcon />
        <CalendarField<CreateShift>
            value={c => c.endDateTime}
            title='End'
            showTime
            showIcon />
    </CommandDialog>
);
