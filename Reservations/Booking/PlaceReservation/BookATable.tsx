import { useParams } from 'react-router-dom';
import { CommandDialog } from '@cratis/components/CommandDialog';
import { InputTextField, CalendarField, NumberField } from '@cratis/components/CommandForm';
import { Guid } from '@cratis/fundamentals';
import { PlaceReservation } from './PlaceReservation';

export const BookATableDialog = () => {
    const { restaurantId } = useParams<{ restaurantId: string }>();

    return (
        <CommandDialog
            command={PlaceReservation}
            title='Book a Table'
            okLabel='Book Table'
            cancelLabel='Cancel'
            onBeforeExecute={command => {
                command.id = Guid.create();
                command.restaurantId = Guid.parse(restaurantId!);
                return command;
            }}>
            <InputTextField<PlaceReservation>
                value={c => c.email}
                title='Email'
                icon={<i className='pi pi-envelope' />} />
            <CalendarField<PlaceReservation>
                value={c => c.start}
                title='Start'
                showTime
                showIcon />
            <CalendarField<PlaceReservation>
                value={c => c.end}
                title='End'
                showTime
                showIcon />
            <NumberField<PlaceReservation>
                value={c => c.partySize}
                title='Party Size'
                min={1} />
        </CommandDialog>
    );
};
