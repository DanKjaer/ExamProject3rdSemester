import {Component, OnInit} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {AnimalFeed, Animals} from "../../models";
import {firstValueFrom} from "rxjs";
import {State} from "../../state";
import {ToastController} from "@ionic/angular";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-species',
  templateUrl: './species.component.html',
  styleUrls: ['./species.component.scss'],
})
export class SpeciesComponent  implements OnInit {

  createNewAnimalForm = this.fb.group({
    speciesID: ['', Validators.required],
    animalName: ['', Validators.required],
    animalBirthday: ['', Validators.required],
    animalGender: ['', Validators.required],
    animalPicture: [''],
    animalWeight: ['', Validators.required],
    animalDead: [false]
  })

  constructor(public fb: FormBuilder, public http : HttpClient, public state: State, public toastController: ToastController, public route: ActivatedRoute) { }

  ngOnInit() {}

  async createAnimal() {
    try {
      let dto = this.createNewAnimalForm.getRawValue();
      this.route.paramMap.subscribe(params => {
        dto.speciesID = params.get('id');
      });
      const observable = this.http.post<Animals>('http://localhost:5000/api/animal', dto);
      const response = await firstValueFrom(observable);
      console.log(response);
      this.state.animalFeed.push(<AnimalFeed>response);
    }catch (e) {
      if (e instanceof HttpErrorResponse) {
        this.toastController.create({message: e.error.messageToCient}).then(res => res.present)
      }
    }
  }

}