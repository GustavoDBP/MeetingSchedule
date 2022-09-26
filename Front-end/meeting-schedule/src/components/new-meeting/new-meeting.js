import React from "react";
import Modal from '@mui/material/Modal';
import './new-meeting.css'
import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, FormControl, InputLabel, MenuItem, Select, TextField } from "@mui/material";
import { DesktopDatePicker, LocalizationProvider, TimePicker } from "@mui/x-date-pickers";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import dayjs from "dayjs";


class NewMeeting extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            name: '',
            description: '',
            roomId: 0,
            start: dayjs(this.props.start),
            end: dayjs(this.props.end),
        };
    }

    saveChanges(success, fail) {
        this.setState({
            isLoading: true
        });
        fetch(
            `https://localhost:44335/api/meetings/`,
            {
                method: 'POST',
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
                        success();
                    }
                    else if (res) {
                        fail(res);
                    }
                },
                (error) => {
                    fail(error);
                }
            );
    };

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
                    <h3>Nova Reunião</h3>
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
                    />
                    <FormControl >
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
                        />
                        <TimePicker
                            label="Início"
                            value={this.state.start}
                            onChange={(value) => {
                                this.setState({ start: value })
                            }}
                            renderInput={(params) => <TextField {...params} />}
                            maxTime={this.state.end}
                        />
                        <TimePicker
                            label="Fim"
                            value={this.state.end}
                            onChange={(value) => { this.setState({ end: value }) }}
                            renderInput={(params) => <TextField {...params} />}
                            minTime={this.state.start}
                        />
                    </LocalizationProvider>
                </div>
                <div className="footer">
                    <Button
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
                    >Agendar</Button>
                </div>
            </div>
        );
    }

    reset() {
        this.setState({
            name: '',
            description: '',
            roomId: 0,
            start: dayjs(this.props.start),
            end: dayjs(this.props.end)
        })
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

export default NewMeeting;