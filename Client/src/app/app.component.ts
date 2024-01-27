import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  // impletments- it will create ngOnInit method
  title = 'Dating app';
  users : any; // any is type of the property

  constructor(private http: HttpClient) {}
  
  ngOnInit(): void { 
    // get - method get something from api server & return an observable
    // subscribe - when we describe a 'next' we get the data and return it to users
    this.http.get('https://localhost:5001/api/users').subscribe({
      next : response => this.users = response,
      error : error => console.log(error),
      complete : () => console.log('Request has completed.')
      
    })

  }

  
  }
  

