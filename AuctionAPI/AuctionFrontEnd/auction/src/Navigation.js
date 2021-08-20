import React,{Component} from 'react';
import {NavLink} from 'react-router-dom';
import {Navbar,Nav} from 'react-bootstrap';
import axios from 'axios';

var getBidderUrl = process.env.REACT_APP_API+"bidder/";

export class Navigation extends Component{
    constructor(props){
        super(props);
        this.state={ userName: "" }
    }

    async componentDidMount(){
        let bidderId = sessionStorage.getItem('bidderId');
        try {
            const response = await axios.get(getBidderUrl + bidderId);
            this.setState({userName:response.data.Name});
          } catch (error) {
            console.error(error);
          }
          
    }

    logout(){
        sessionStorage.removeItem('bidderId');
        sessionStorage.removeItem('token');
        window.location.href = '/';
    }

    render(){
        return(
            <Navbar style={{background:"#4287f5"}} expand="lg">
                <Navbar.Toggle aria-controls="basic-navbar-nav"/>
                <Navbar.Collapse id="basic-navbar-nav">
                    <Nav className="d-flex justify-content-between w-100 align-items-baseline">
                        <NavLink className="d-inline text-white bd-dark p-2" to="/">Home</NavLink>
                        <div>
                            <h6 className="mx-2 mb-0 text-white">{this.state.userName}</h6>
                            <p style={{ cursor: 'pointer' }} onClick={this.logout} className="d-inline text-white bd-dark p-2">Logout</p>
                        </div>
                    </Nav>
                </Navbar.Collapse>
            </Navbar>
        )
    }
}