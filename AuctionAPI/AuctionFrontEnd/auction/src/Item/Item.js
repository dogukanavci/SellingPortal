import React,{Component} from 'react';
import axios from 'axios';

import { Util } from '../Utility';
import {BidsTable} from './ItemComponents/BidsTable/BidsTable';
import {CountDown} from './ItemComponents/CountDown/CountDown';
import AutoBidInformation from './ItemComponents/AutoBidInformation';

import './Item.scss';

var getItemUrl = process.env.REACT_APP_API+"item/";
var getBidsUrl = process.env.REACT_APP_API+"bid/GetBidsForItem/";
var postBidUrl = process.env.REACT_APP_API+"bid/";
var autoBidSetUrl = process.env.REACT_APP_API+"bid/SetAutobid/";
var getAutoBidUrl = process.env.REACT_APP_API+"bid/GetAutoBid/";

export class Item extends Component{
    constructor(props){
        super(props);
        this.state={ id: this.props.match.params.id, image: null, name: null, description: null, expiry: null, bids: [], bidAmount : "", bidderId: sessionStorage.getItem('bidderId'), autoBidAmount: -1, autoBidding: false}
    }

    async onBid() {
        let bid = {BidderId : this.state.bidderId, ItemId :this.state.id, Offer: this.state.bidAmount, ItemExpiry: this.state.expiry}
        try {
            await axios.post(postBidUrl, bid);
            await this.getBids();
          } catch (error) {
            console.error(error);
          }
    }

    async setAutoBidMood(mod) {
        try {
            let url = autoBidSetUrl + this.state.bidderId + "/" + this.state.id + "/" + this.state.autoBidAmount + "/" + mod;
            const response = await axios.get( url  );
            this.setState({
                autoBidAmount: response.data.Budget,
                autoBidding: response.data.Mode
            });
            await this.getBids();
            await this.getAutoBid();
          } catch (error) {
            console.error(error);
          }
    }

    updateBidInputValue(evt) {
        this.setState({
          bidAmount: evt.target.value
        });
    }

    updateAutoBidInputValue(evt) {
        this.setState({
            autoBidAmount: evt.target.value
        });
    }

    async getItem(){
        try {
            const response = await axios.get(getItemUrl + this.state.id);
            let data = response.data;
            this.setState({ id: data.Id, image: data.Image, name: data.Name,
                 description: data.Description,expiry: data.Expiry});
          } catch (error) {
            console.error(error);
          }
    }
    processBidList(data){
        for (var key in data.Bids) {
            var bid = data.Bids[key];
            bid.Name = data.BidderNameLookup[bid.BidderId];
            delete bid.BidderId
        }
    }

    async getAutoBid(){
        try {
            const response = await axios.get(getAutoBidUrl + this.state.bidderId + "/" + this.state.id);
            let data = response.data;
            this.setState({ autoBidAmount: data.Budget,autoBidding: data.BiddingForItem});
          } catch (error) {
            console.error(error);
          }
    }

    async getBids(){
        try {
            const response = await axios.get(getBidsUrl + this.state.id);
            let data = response.data;
            this.processBidList(data);
            this.setState({ bids: data.Bids});
          } catch (error) {
            console.error(error);
          }
    }

    async componentDidMount(){
        await this.getItem();
        await this.getBids();
        await this.getAutoBid();
    }

    render(){
        const { image, name, description, expiry, bids,bidAmount, autoBidAmount,autoBidding } = this.state;
        if(expiry === null) return null;
        return(
            <div className="main-item-container m-5 pt-2">
                <h3 className="text-center my-3">{name}</h3>
                <div className="d-flex justify-content-start px-5 pb-5">
                    <img alt="Item" src={`data:image/jpeg;charset=utf-8;base64,${image}`} />
                    <div className="mx-3 w-100">
                        <h5 className="mb-2 text-center">Description</h5>
                        <p className="mb-2">{description}</p>
                        <CountDown countDown={Util.decomposeDate(new Date(expiry))} />
                        <BidsTable bids={bids}/>
                            {!autoBidding ? 
                                (
                                    <div>
                                        <div className="d-flex justify-content-center align-items-center mt-3">
                                        <label>Bid Amount</label>
                                            <input className="mx-3" style={{height:"25px"}} type="value" aria-label="Bid Now" value={bidAmount} onChange={evt => this.updateBidInputValue(evt)}/>
                                            <button onClick={async () => {await this.onBid();}}>Bid Now</button>
                                        </div>
                                        <div className="d-flex justify-content-center align-items-center mt-3">
                                            <label>Total Autobid Budget</label>
                                            <input className="mx-3" style={{height:"25px"}} type="value" aria-label="Autobid Amount" value={autoBidAmount} onChange={evt => this.updateAutoBidInputValue(evt)}/>
                                            <button className="activate-button" onClick={async () => {await this.setAutoBidMood("true");}}>Activate Autobid</button>
                                        </div>
                                    </div>
                                ) :
                                (
                                    <div className="d-flex justify-content-center align-items-center mt-3">
                                        <label>Total Autobid Budget</label>
                                        <input readOnly className="mx-3 deactivate-input" style={{height:"25px"}} type="value" aria-label="Autobid Amount" value={autoBidAmount}/>
                                        <button className="deactivate-button" onClick={async () => {await this.setAutoBidMood("false");}}>Deactivate Autobid</button>
                                    </div>
                                )
                            }
                            <div className="d-flex justify-content-center mt-3"><AutoBidInformation/></div>
                        
                    </div>
                </div>
            </div>
            
            
        )
    }
}