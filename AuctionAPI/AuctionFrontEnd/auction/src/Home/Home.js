import React,{Component} from 'react';
import axios from 'axios';

import {ItemElement} from './ItemElement/ItemElement';
import Pagination from './Pagination';
import {ItemSearch} from './ItemSearch';
import {ItemSort} from './ItemSort';

import './Home.scss';

var pageSize = 10;
var getItemsUrl = process.env.REACT_APP_API+"item/getItems";

export class Home extends Component{
    constructor(props){
        super(props);
        this.state={ totalItems: null, currentItems: [], currentPage: null, totalPages: null, searchTerm: "", sortBy: "Expiry" }
    }

    onPaginate = data => {
        const { currentPage } = data;
            
        axios.post( getItemsUrl, {PageIndex: currentPage,PageSize: pageSize,SearchTerm: this.state.searchTerm,SortBy: this.state.sortBy})
          .then(res => {
            const currentItems = res.data.Items;
            let totalItems = res.data.TotalCount;
            let totalPages = 1 + (totalItems - totalItems % pageSize) / pageSize;
            this.setState({totalItems:totalItems, currentPage:currentPage, currentItems:currentItems, totalPages:totalPages });
        })
        .catch(function (error) {
            console.log(error);
        })
    }

    onSearch = searchTerm => {
        axios.post( getItemsUrl, {PageIndex: this.state.currentPage,PageSize: pageSize,SearchTerm: searchTerm,SortBy: this.state.sortBy})
          .then(res => {
            const currentItems = res.data.Items;
            let totalItems = res.data.TotalCount;
            let totalPages = 1 + (totalItems - totalItems % pageSize) / pageSize;
            this.setState({totalItems:totalItems, currentItems:currentItems, searchTerm:searchTerm, totalPages:totalPages });
        })
        .catch(function (error) {
            console.log(error);
        })
    }

    onSort = sortBy => {
        axios.post( getItemsUrl, {PageIndex: this.state.currentPage,PageSize: pageSize,SearchTerm: this.state.searchTerm,SortBy: sortBy})
          .then(res => {
            const currentItems = res.data.Items;
            let totalItems = res.data.TotalCount;
            let totalPages = 1 + (totalItems - totalItems % pageSize) / pageSize;
            this.setState({totalItems:totalItems, currentItems:currentItems, sortBy:sortBy, totalPages:totalPages});
        })
        .catch(function (error) {
            console.log(error);
        })
    }

    async componentDidMount(){
        let data = {PageIndex: 1,PageSize: pageSize,SearchTerm: '',SortBy: 'Expiry'}
        try {
            const response = await axios.post(getItemsUrl,data);
            let totalItems = response.data.TotalCount;
            let totalPages = 1 + (totalItems - totalItems % pageSize) / pageSize;
            this.setState({totalItems:totalItems, currentItems:response.data.Items, currentPage:1, totalPages:totalPages});
          } catch (error) {
            console.error(error);
          }
          
    }

    componentDidUpdate(){
        //this.refreshList();
    }

    render(){
        const { totalItems, currentItems, searchTerm, sortBy } = this.state;
        if(totalItems == null) return null;
        return(
            <div className="m-5 home-container">
                <div className="d-flex justify-content-between align-items-center">
                    <div className="d-flex justify-content-start align-items-center">
                        <ItemSearch className="mr-2" searchTerm={searchTerm} onSearch={this.onSearch}/>
                        <ItemSort sortBy={sortBy} onSort={this.onSort}/>
                    </div>
                    <div className="d-flex">
                        <Pagination totalRecords={totalItems} pageLimit={pageSize} pageNeighbours={1} onPageChanged={this.onPaginate} />
                    </div>
                </div>
                <div className="gallery-grid">
                    {currentItems.map(item =>
                        <ItemElement key={item.Id} item={item}/>
                    )}
                </div>
            </div>
            
            
        )
    }
}