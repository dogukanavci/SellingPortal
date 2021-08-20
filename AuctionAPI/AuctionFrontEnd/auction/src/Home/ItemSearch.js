import React, { Component, Fragment } from 'react';

export class ItemSearch extends Component {
    constructor(props) {
        super(props);
        this.state = {
            searchTerm: props.searchTerm
        };
    }

    onSearch = () => {
        this.props.onSearch(this.state.searchTerm);
    }

    updateInputValue(evt) {
        this.setState({
          searchTerm: evt.target.value
        });
      }

    render() {
        return (
          <Fragment>
                <input style={{height:"25px"}} type="search" aria-label="Search Items" value={this.state.searchTerm} onChange={evt => this.updateInputValue(evt)}/>
                <button onClick={this.onSearch}>Search</button>
          </Fragment>
        );
    }
}