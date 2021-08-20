import React,{Component} from 'react';
import {Link} from 'react-router-dom';
import {Util} from '../../Utility';
import {CountDown} from '../../Item/ItemComponents/CountDown/CountDown';
import './ItemElement.scss';

export class ItemElement extends Component{
    render(){
        let redirectUrl = "/item/" + this.props.item.Id;
        return(
            <div key={this.props.item.Id} className="item-container">
                <h5 className="text-center my-3">{this.props.item.Name}</h5>
                <div className="d-flex justify-content-start">
                    <img alt="Item" src={`data:image/jpeg;charset=utf-8;base64,${this.props.item.Image}`} />
                    <div className="mx-3">
                        <h6 className="my-2">Description</h6>
                        <p>{this.props.item.Description}</p>
                    </div>
                </div>
                <div className="bid-display">
                    <h6 className="my-2 mx-2">Last Bid</h6>
                    <p className="m-0">{this.props.item.LastPrice}</p>
                </div>
                
                <CountDown countDown={Util.decomposeDate(new Date(this.props.item.Expiry))} />
                <div className="d-flex justify-content-center"><Link className="mb-3" to={redirectUrl} >Bid Now</Link></div>
                
            </div>
            
        )
    }
}