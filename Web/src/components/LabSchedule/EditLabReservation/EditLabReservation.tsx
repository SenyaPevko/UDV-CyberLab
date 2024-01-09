import Modal from 'react-bootstrap/Modal';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';

import React, { useState } from 'react';
import { Button as LocalButton } from '../../Button/Button';
import { apiService } from '../../../services';
import {CreateLabReservationRequest, LabReservation, UpdateLabReservationRequest } from '../../../../api';
import { userStore } from '../../../stores';
import {convertDateToNetTicks} from "../AddLabReservation/AddLabReservation"

import style from './EditLabReservation.module.scss'

interface Props {
  show: boolean;
  handleClose: () => void;
  selectedReservation: LabReservation;
  updateTable: () => void
}

export const EditLabReservation: React.FC<Props> = ({
  show,
  handleClose,
  selectedReservation,
  updateTable,
}) => {
  const [timeStart, setTimeStart] = useState(new Date());
  const [timeEnd, setTimeEnd] = useState(new Date());
  const [theme, setTheme] = useState('');
  const [description, setDescription] = useState('');

  const handleSaveReservation = async () => {
    const updateRequest: UpdateLabReservationRequest = {
      id: selectedReservation.id,
      timeStart: convertDateToNetTicks(timeStart),
      timeEnd: convertDateToNetTicks(timeEnd),
      theme,
      description,
      reservorId: selectedReservation.reservor.id,
      lab: selectedReservation?.lab,
      currentUserId: userStore.user?.id,
    };
    const response = await apiService.updateLabReservation(updateRequest);
    if (!(response instanceof Error)) {
      handleClose();
      updateTable();
    } else {
      // Handle error case, e.g., show error message
    }
  };

  return (
    <Modal className={style.addReservationOverlay} show={show} onHide={handleClose} dialogClassName={style.addReservationModal}>
      <Modal.Header closeButton>
        <Modal.Title>Изменить резервацию</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <form className={style.addReservationForm}>
        <div className={style.formGroup}>
              <label>Тема:</label>
              <input type="text" value={theme} onChange={(e) => setTheme(e.target.value)} />
            </div>
            <div className={style.formGroup}>
              <label>Описание:</label>
              <textarea value={description} onChange={(e) => setDescription(e.target.value)} />
            </div>
            <div className={style.timeInputs}>
              <div className={style.formGroup}>
                <label>Время начала:</label>
                <DatePicker
            selected={timeStart}
            onChange={(date: Date) => setTimeStart(date)}
            showTimeSelect
            timeFormat="HH:mm"
            dateFormat="MMMM d, yyyy h:mm aa"
            className={style.datePicker}
          />
              </div>
              <div className={style.formGroup}>
                <label>Время конца:</label>
                <DatePicker
            selected={timeEnd}
            onChange={(date: Date) => setTimeEnd(date)}
            showTimeSelect
            timeFormat="HH:mm"
            dateFormat="MMMM d, yyyy h:mm aa"
            className={style.datePicker}
          />
              </div>
            </div>
        </form>
      </Modal.Body>
      <Modal.Footer>
        <LocalButton variant="primary" onClick={handleSaveReservation}>
          Сохранить
        </LocalButton>
        <LocalButton variant="secondary" onClick={handleClose}>
          Отменить
        </LocalButton>
      </Modal.Footer>
    </Modal>
  );
};