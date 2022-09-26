import React from "react";
import Modal from '@mui/material/Modal';
import './meeting-details.css'
import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, FormControl, InputLabel, MenuItem, Select, TextField } from "@mui/material";
import { DesktopDatePicker, LocalizationProvider, TimePicker } from "@mui/x-date-pickers";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import dayjs from "dayjs";


class MeetingDetails extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            isEditing: false,
            isLoading: false,
            confirmationDialog: false,
            name: this.props.meeting.name,
            description: this.props.meeting.description,
            roomId: this.props.meeting.room.id,
            start: dayjs(this.props.meeting.start),
            end: dayjs(this.props.meeting.end),
        };
    }

    saveChanges(success, fail) {
        this.setState({
            isLoading: true
        });
        fetch(
            `https://localhost:44335/api/meetings/${this.props.meeting.id}`,
            {
                method: 'PATCH',
                body: JSON.stringify({
                    name: this.state.name,
                    description: this.state.description,
                    roomId: this.state.roomId,
                    start: this.state.start.add(-3, 'hour'), // Remove 3h provisóriamente. Isso irá ser alterado futuramente.
                    end: this.state.end.add(-3, 'hour')
                }),
                headers: {
                    'Content-type': 'application/json;',
                },
            })
            .then(
                (res) => {
                    if (res.ok) {
                        res.json().then((json) => {
                            this.setState({
                                isLoading: false,
                                isEditing: false,
                                roomId: json.room.id
                            });
                            this.props.meeting.name = this.state.name;
                            this.props.meeting.description = this.state.description;
                            this.props.meeting.room = this.props.rooms.find((room) => { return room.id === this.state.roomId });
                            this.props.meeting.start = this.state.start;
                            this.props.meeting.end = this.state.end;
                            success();
                        })
                    }
                    else if (res) {
                        this.setState({
                            isLoading: false,
                            isEditing: true
                        });
                        fail(res);
                    }
                },
                (error) => {
                    this.setState({
                        isLoading: false,
                        isEditing: true
                    });
                    fail(error);
                }
            );
    };

    remove(success, fail) {
        this.setState({
            isLoading: true
        });
        fetch(
            `https://localhost:44335/api/meetings/${this.props.meeting.id}`,
            {
                method: 'DELETE'
            })
            .then(
                () => {
                    this.setState({
                        isLoading: false,
                        isEditing: false
                    });
                    success();
                },
                (error) => {
                    this.setState({
                        isLoading: false,
                        isEditing: true
                    });
                    fail(error);
                }
            );
    }

    reset() {
        this.setState({
            isEditing: false,
            isLoading: false,
            name: this.props.meeting.name,
            description: this.props.meeting.description,
            roomId: this.props.meeting.room.id,
            start: dayjs(this.props.meeting.start),
            end: dayjs(this.props.meeting.end),
        });
    }

    renderRooms() {
        this.props.rooms.map((room, index) => {
            return (
                <MenuItem
                    value={room.id}
                    key={index}
                >asdas</MenuItem>
            )
        });
    }

    renderContent() {
        return (
            <div className='modal-card'>
                <div className='header'>
                    <h3>{this.props.meeting.name}</h3>
                </div>
                <div className='content'>
                    <TextField
                        id="name"
                        label="Nome"
                        variant="standard"
                        value={this.state.name}
                        onChange={(event) => {
                            let value = event.target.value;
                            value = value.slice(0, 20)
                            this.setState({ name: value });
                        }}
                        disabled={!this.state.isEditing}
                    />
                    <TextField
                        id="description"
                        label="Descrição"
                        variant="standard"
                        value={this.state.description}
                        maxRows={6}
                        multiline
                        onChange={(event) => {
                            let value = event.target.value;
                            this.setState({ description: value });
                        }}
                        disabled={!this.state.isEditing}
                    />
                    <FormControl disabled={!this.state.isEditing}>
                        <InputLabel>Sala</InputLabel>
                        <Select
                            value={this.state.roomId}
                            label="Sala"
                            onChange={(event) => { this.setState({ roomId: event.target.value }) }}
                        >
                            <MenuItem value={0}>Seleção automática</MenuItem>
                            {this.props.rooms.map((room, index) => {
                                return (
                                    <MenuItem
                                        value={room.id}
                                        key={index}
                                    >{room.name}</MenuItem>
                                )
                            })}
                        </Select>
                    </FormControl>
                    <LocalizationProvider dateAdapter={AdapterDayjs}>
                        <DesktopDatePicker
                            label="Data"
                            inputFormat="DD/MM/YYYY"
                            value={this.state.start}
                            onChange={(value) => {
                                let newStart = dayjs(this.state.start)
                                    .set('date', value.get('date'))
                                    .set('month', value.get('month'))
                                    .set('year', value.get('year'));

                                let newEnd = dayjs(this.state.end)
                                    .set('date', value.get('date'))
                                    .set('month', value.get('month'))
                                    .set('year', value.get('year'));

                                this.setState({ start: newStart, end: newEnd });
                            }}
                            renderInput={(params) => <TextField {...params} />}
                            disabled={!this.state.isEditing}
                        />
                        <TimePicker
                            label="Início"
                            value={this.state.start}
                            onChange={(value) => {
                                this.setState({ start: value })
                            }}
                            renderInput={(params) => <TextField {...params} />}
                            maxTime={this.state.end}
                            disabled={!this.state.isEditing}
                        />
                        <TimePicker
                            label="Fim"
                            value={this.state.end}
                            onChange={(value) => { this.setState({ end: value }) }}
                            renderInput={(params) => <TextField {...params} />}
                            minTime={this.state.start}
                            disabled={!this.state.isEditing}
                        />
                    </LocalizationProvider>
                </div>
                <div className="footer">
                    <div hidden={this.state.isEditing}>
                        <Button hidden
                            disabled={this.state.isLoading}
                            onClick={
                                () => {
                                    this.setState({ isEditing: true });
                                }}
                        >Editar</Button>
                    </div>
                    <div hidden={!this.state.isEditing}>
                        <Button
                            disabled={this.state.isLoading}
                            onClick={
                                () => {
                                    this.saveChanges(
                                        () => {
                                            this.props.onSave();
                                        },
                                        (error) => {
                                            if (error.status === 409) {
                                                error.json()
                                                    .then((json) => {
                                                        if (this.state.roomId === 0)
                                                            alert("Não existem salas disponíveis para o horário informado");
                                                        else
                                                            alert("A sala não está disponível no horário informado");
                                                    })
                                            }
                                            else
                                                alert("Ocorreu um erro ao salvar as alterações");
                                        }
                                    )
                                }}
                        >Salvar</Button>
                        <Button
                            disabled={this.state.isLoading}
                            color="error"
                            onClick={
                                () => {
                                    this.setState({ confirmationDialog: true })
                                }}
                        >Remover</Button>
                    </div>
                </div>
            </div>
        );
    }

    render() {
        if (!this.state.confirmationDialog) {
            return (
                <Modal
                    open={this.props.isOpen}
                    onClose={() => {
                        this.reset();
                        this.props.onClose();
                    }}
                >
                    <div>
                        {this.renderContent()}
                    </div>
                </Modal>
            )
        }
        else {
            return (
                <Dialog
                    open={this.state.confirmationDialog}
                    onClose={() => { }}
                >
                    <DialogTitle>
                        {"Você deseja remover este agendamento?"}
                    </DialogTitle>
                    <DialogContent>
                        <DialogContentText>
                            A remoção de um agendamento não pode ser revertida.
                        </DialogContentText>
                    </DialogContent>
                    <DialogActions>
                        <Button
                            onClick={() => {
                                this.setState({ confirmationDialog: false })
                            }}
                        >Não</Button>

                        <Button
                            autoFocus
                            onClick={() => {
                                this.remove(
                                    () => {
                                        this.props.onRemove();
                                        this.setState({ confirmationDialog: false });
                                    },
                                    (error) => {
                                        this.alert("Ocorreu um erro ao remover o agendamento");
                                        this.setState({ confirmationDialog: false });
                                    }
                                )
                            }}
                        >Sim</Button>
                    </DialogActions>
                </Dialog >
            )
        }
    }
}

export default MeetingDetails;