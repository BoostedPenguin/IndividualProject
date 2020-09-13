// Import dependencies
import React from 'react';
import { Button, FormControl, Row, Col, Container } from 'react-bootstrap';
import './FirstComponent.css'
import 'bootstrap/dist/css/bootstrap.min.css';


// Render component
class FirstComponent extends React.Component {
    render() {
        return (
            <Container>
                <Row className="mt-4">
                    <Col className="col-12 col-sm-10">
                        <FormControl name="name"></FormControl>
                    </Col>
                    <Col>
                        <Button variant="secondary" className="btn-block mt-3 mt-sm-0">Primary</Button>
                    </Col>
                </Row>
            </Container>
        )
    }
}

// Variables


// Methods


function DisplayName(name) {
    if(name) {
        return name;
    }
    return "Stranger";
}

function DisplayDate() {
    return new Date().toLocaleString();
}

// Export component
export default FirstComponent;