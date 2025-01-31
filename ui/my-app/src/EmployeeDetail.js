import React,{Component} from 'react';
import {variables} from './Variables.js';
import { withRouter } from "react-router";

class EmployeeDetail extends React.Component {
    constructor(props){
        super(props);

        this.state={
            departments:[],
            employees:[],
            modalTitle:"",
            EmployeeId:0,
            EmployeeName:"",
            Department:"",
            DateOfJoining:"",
            PhotoFileName:"anonymous.png",
            PhotoPath:variables.PHOTO_URL
        }
    }

    componentDidMount() {
        const id = this.props.match.params.id;
        this.fetchData(id);
    }

    fetchData = id => {
        fetch(variables.API_URL+'employee/'+id)
        .then(response=>response.json())
        .then(data=>{
            this.setState({employees:data});
        });

        fetch(variables.API_URL+'department')
        .then(response=>response.json())
        .then(data=>{
            this.setState({departments:data});
        });
    };

    render() { const {
        departments,
        employees,
        modalTitle,
        EmployeeId,
        EmployeeName,
        Department,
        DateOfJoining,
        PhotoPath,
        PhotoFileName
    }=this.state;
        return (<div>{employees.map(emp=><h2>Employee {emp.EmployeeName}</h2>)}
        <br />
        {employees.map(emp=><img src={PhotoPath+emp.PhotoFileName} />)}
        <table className="table table-striped">
        <thead>
        <tr>
            <th>
                EmployeeId
            </th>
            <th>
                EmployeeName
            </th>
            <th>
                Department
            </th>
            <th>
                DOJ
            </th>
        </tr>
        </thead>
        <tbody>
            {employees.map(emp=>
                <tr key={emp.EmployeeId}>
                    <td>{emp.EmployeeId}</td>
                    <td>{emp.EmployeeName}</td>
                    <td>{emp.Department}</td>
                    <td>{emp.DateOfJoining}</td>
                </tr>
                )}
        </tbody>
        </table></div>)
    }
}

export default withRouter(EmployeeDetail);