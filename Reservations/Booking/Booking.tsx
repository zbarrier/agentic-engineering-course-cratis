import { Button } from 'primereact/button';
import { BookATableDialog } from './PlaceReservation';
import { useDialog } from '@cratis/arc.react/dialogs';

export const Booking = () => {
    const [BookATable, showBookATable] = useDialog(BookATableDialog);

    return (
        <div className='p-4'>
            <div className='flex justify-between items-center mb-4'>
                <h1 className='text-2xl font-bold'>Reservations</h1>
                <Button
                    label='Book a Table'
                    icon='pi pi-plus'
                    onClick={() => showBookATable()} />
            </div>
            <BookATable />
        </div>
    );
};
