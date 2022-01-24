import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ApiService } from 'src/app/api.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  forma = new FormGroup({
    vardas : new FormControl('', Validators.required),
    slaptazodis : new FormControl('', Validators.required),
    slaptazodis2 : new FormControl('', Validators.required),
    pastas : new FormControl('', [Validators.required, Validators.email])
  })

  constructor(private apiService : ApiService) { }

  ngOnInit(): void {
  }

  registruotis(){
    if(this.forma.value.slaptazodis != this.forma.value.slaptazodis2){
      alert("Slaptazodziai nesutampa !")
    }
    else{
      console.log(this.forma.value)
      this.apiService.registruotis(this.forma.value)
      // https://localhost:44321/login
    }
  }

}
