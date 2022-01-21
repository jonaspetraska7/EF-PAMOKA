import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ApiService } from 'src/app/api.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  forma = new FormGroup({
    vardas : new FormControl('', Validators.required),
    slaptazodis : new FormControl('', Validators.required)
  })

  constructor(private apiService : ApiService) { }

  ngOnInit(): void {
  }

  prisijungti(){
    console.log(this.forma.value)
    this.apiService.prisijungti(this.forma.value)
    // https://localhost:44321/login
  }



}
