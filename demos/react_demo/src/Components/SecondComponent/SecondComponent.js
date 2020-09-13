import React from 'react';
import { Button, FormControl, Row, Col, Container } from 'react-bootstrap';
import './SecondComponent.css'
import 'bootstrap/dist/css/bootstrap.min.css';

class SecondComponent extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            date: new Date(),
            inputValue: ''
        };

        //
        this.DisplayInput = this.DisplayInput.bind(this);
    }

    //On mounted
    componentDidMount() {
        this.timerID = setInterval(() => {
            this.tick();
        }, 1000);
    }

    // On unmounted
    componentWillUnmount() {
        clearInterval(this.timerID);
    }

    // Set the new state
    tick() {
        this.setState({
            date: new Date()
        });
    }

    DisplayInput(event) {
        this.setState({inputValue: event.target.value})
    }

    render() {
        return (
            <div>
                <Container className="mt-5">
                    <FormControl placeholder="Placeholder" onChange={this.DisplayInput}></FormControl>
                </Container>
                <h2>Input is: {this.state.inputValue}</h2>
                <h2>It is {this.state.date.toLocaleTimeString()}.</h2>
          </div>
        );
    }
}

export default SecondComponent;