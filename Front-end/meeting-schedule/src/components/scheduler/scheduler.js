import React, { useState } from 'react';
import { IconButton, LinearProgress, Tooltip, Zoom } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import './scheduler.css';
import { DatePicker, LocalizationProvider } from '@mui/x-date-pickers';
import TextField from '@mui/material/TextField';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import MeetingDetails from '../meeting-details/meeting-details';
import NewMeeting from '../new-meeting/new-meeting';
import dayjs from 'dayjs';

class Scheduler extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            error: null,
            isLoaded: false,
            meetings: [],
            rooms: [],
            date: dayjs(),
            meetingDetailsModal: {
                open: false,
                meeting: null
            }
        };
    }

    Day = (props) => {
        const [modalOpen, setModalOpen] = useState(false);

        const today = new Date(new Date().toDateString());
        let items = [];
        for (const meetingIndex in props.meetings) {
            const meeting = props.meetings[meetingIndex];
            items.push(
                <this.Meeting meeting={meeting} key={meeting.id} />
            );
        }
        return (
            <div className={`day ${new Date(props.date) < today ? 'past' : ''} ${new Date(props.date).toDateString() === today.toDateString() ? 'today' : ''}`} >
                <div className='header'>
                    <span>{props.date.getDate()}</span>
                </div>
                <div className='content'>
                    {items}
                </div>
                <div className='footer'>
                    <Tooltip title="Agendar reunião">
                        <IconButton
                            onClick={() => { setModalOpen(true) }}
                        >
                            <AddIcon></AddIcon>
                        </IconButton>
                    </Tooltip>
                </div>
                <NewMeeting
                    isOpen={modalOpen}
                    onClose={() => { setModalOpen(false); }}
                    onSave={() => {
                        setModalOpen(false);
                        this.refresh();
                    }}
                    start={dayjs(props.date).add(8, 'hour')}
                    end={dayjs(props.date).add(9, 'hour')}
                    rooms={this.state.rooms}
                >
                </NewMeeting>
            </div>
        )
    }

    Meeting = (props) => {
        const [modalOpen, setModalOpen] = useState(false);

        const now = new Date();
        const room = this.state.rooms.find(room => room.id === props.meeting.room.id);
        const start = new Date(props.meeting.start);
        const end = new Date(props.meeting.end);
        return (
            <div>
                <Tooltip title={room.name} arrow TransitionComponent={Zoom}>
                    <div
                        className={`meeting ${start < now ? 'past' : ''}`}
                        style={{ backgroundColor: room.color }}
                        onClick={() => {
                            setModalOpen(true);
                        }}
                    >
                        <span>{`${start.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })} - ${end.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}`}</span>
                    </div>
                </Tooltip>
                <MeetingDetails
                    meeting={props.meeting}
                    isOpen={modalOpen}
                    onClose={() => { setModalOpen(false); }}
                    onSave={() => { this.refresh() }}
                    onRemove={() => {
                        this.refresh();
                    }}
                    rooms={this.state.rooms}
                ></MeetingDetails>
            </div >
        )
    }

    componentDidMount() {
        this.refresh();
    }

    async refresh() {
        let rooms;
        let meetings;

        await fetch("https://localhost:44335/api/rooms")
            .then(res => res.json())
            .then(
                (result) => {
                    rooms = result;
                },
                (error) => {
                    this.setState({
                        isLoaded: true,
                        error
                    });
                    return;
                }
            );

        await fetch(`https://localhost:44335/api/meetings/calendar-view/${this.state.date.toISOString()}`)
            .then(res => res.json())
            .then(
                (result) => {
                    meetings = result;
                },
                (error) => {
                    this.setState({
                        isLoaded: true,
                        error
                    });
                    return;
                }
            );

        this.setState({
            isLoaded: true,
            meetings,
            rooms
        });
    }

    render() {
        const numberOfDays = new Date(this.state.date.get('year'), this.state.date.get('month') + 1, 0).getDate();

        let items = []
        for (let dayNumber = 1; dayNumber <= numberOfDays; dayNumber++) {
            const date = new Date(this.state.date.get('year'), this.state.date.get('month'), dayNumber);
            const meetings = this.state.meetings.filter(meeting => new Date(meeting.start).toDateString() === date.toDateString());
            items.push(
                <this.Day date={date} meetings={meetings} key={date} />
            );
        }

        return (
            <div className='scheduler' >
                <div style={{ display: this.state.isLoaded ? 'none' : 'block' }} >
                    <LinearProgress />
                </div>
                <div className='filter' style={{ display: this.state.isLoaded ? 'block' : 'none' }}>
                    <LocalizationProvider dateAdapter={AdapterDayjs}>
                        <DatePicker
                            views={['year', 'month']}
                            label="Ano e Mês"
                            minDate={new Date('2012-03-01')}
                            maxDate={new Date('2023-06-01')}
                            value={this.state.date}
                            onChange={(value) => { this.setState({ date: value }); this.refresh() }}
                            renderInput={(params) => <TextField  {...params} helperText={null} />}
                            openTo={'year'}
                        />
                    </LocalizationProvider>
                </div>
                <div className='days' style={{ display: this.state.isLoaded ? 'flex' : 'none' }}>
                    {items}
                </div>
            </div>
        );
    }
}

export default Scheduler;