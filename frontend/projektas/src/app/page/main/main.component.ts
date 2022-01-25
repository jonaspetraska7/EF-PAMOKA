import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/api.service';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {

  constructor(private apiService : ApiService) { }

  automobiliai : any[] = []

  ngOnInit(): void {
    let test = { Id : "0", marke: "Testas", modelis : "X", SavininkoId : "0" }
    this.automobiliai.push(test)
    this.automobiliai.push(test)
    this.gautiAutomobilius()
  }

  atsijungti(){
    this.apiService.atsijungti();
  }

  async gautiAutomobilius(){
    let atsakymasIsServerio = await fetch("https://localhost:7065/automobiliai", {
      headers : {
        'Content-type': 'application/json',
        'Authorization': `Bearer ${this.apiService.token}`,
      }
    })
    console.log(atsakymasIsServerio)
    let gautiAutomobiliai = await atsakymasIsServerio.json();
    console.log(gautiAutomobiliai)
    if(atsakymasIsServerio.status == 401){
      this.automobiliai = []
    }

    if(gautiAutomobiliai != null && atsakymasIsServerio.status == 200){
      this.automobiliai = gautiAutomobiliai
    }
  }

}
