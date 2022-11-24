//import React, { useState } from 'react';
//import {Inquiry} from "inquiry.jsx"

function Inquiry() {
    return (
        <div>
            <div class="list-group">
                <a href="#" class="list-group-item list-group-item-action active">Cras justo odio</a>
                <a href="#" class="list-group-item list-group-item-action">Inquiry number one</a>
                <a href="#" class="list-group-item list-group-item-action">Inquiry number one</a>
                <a href="#" class="list-group-item list-group-item-action">Inquiry number one</a>
                <a href="#" class="list-group-item list-group-item-action">Inquiry number one</a>
            </div>
        </div>
    )
}


function Inquiries() {

    return (
        <div>
            <Inquiry />
        </div>
    );

}

ReactDOM.render(<Inquiries />, document.getElementById('content2'));