import { useState } from 'react';
import {Modal,Button} from 'react-bootstrap';

export default function AutoBidInformation() {
    const [show, setShow] = useState(false);
  
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
  
    return (
      <>
        <Button variant="primary" onClick={handleShow}>
          More About Autobidding
        </Button>
  
        <Modal show={show} onHide={handleClose}>
          <Modal.Header closeButton>
            <Modal.Title>Autobidding Information</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            Autobidding is a way to ensure staying up to date with recent bids automatically. It increases the most recent bid by one as long as 
            the set budget allows. The box next to the autobidding button lets the user to allocate a budget for all the operations.
            The set budget is shared among other items that has autobidding enabled. Deactivating autobidding for an item does not
            reallocate the bid amount back to the budget to protect the user from being charged more than the initial allocation.
            </Modal.Body>
          <Modal.Footer>
            <Button variant="secondary" onClick={handleClose}>
              Close
            </Button>
          </Modal.Footer>
        </Modal>
      </>
    );
  }