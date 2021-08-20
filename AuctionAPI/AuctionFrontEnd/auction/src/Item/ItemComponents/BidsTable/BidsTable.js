import React,{Component} from 'react';

import { Util } from '../../../Utility';
import './BidsTable.scss';

export class BidsTable extends Component{
    render(){
        return(
            <table>
                <thead>
                    <tr>
                        <th>Amount</th>
                        <th>Bidder</th>
                        <th>Time</th>
                    </tr>
                </thead>
                <tbody>
                    {this.props.bids.map(bid =>
                        <tr key={bid.Id} className={bid.AutoBid ? "autobid" : ""}>
                            <td>{bid.Offer}</td>
                            <td>{bid.Name} {bid.AutoBid ? " (auto)" : ""}</td>
                            <td>{Util.formatDate(new Date(bid.TimeOfBidding))}</td>
                        </tr>
                    )}
                </tbody>
            </table>
            
        )
    }
}