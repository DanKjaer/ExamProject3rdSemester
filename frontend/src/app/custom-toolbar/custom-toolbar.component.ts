import {Component, OnInit, ViewChild} from '@angular/core';
import {animate, style, transition, trigger} from "@angular/animations";
import {IonSearchbar} from "@ionic/angular";
import {TokenService} from 'src/services/token.services';
import {firstValueFrom} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {State} from "../../state";
import {
  AnimalFeed,
  Animals,
  AnimalSearchFeed,
  AnimalSpeciesFeed,
  AnimalSpeciesSearchFeed,
  SearchFeed
} from "../../models";
import {Router} from "@angular/router";
import {FormBuilder, FormControl, Validators} from "@angular/forms";

@Component({
  selector: 'app-custom-toolbar',
  templateUrl: './custom-toolbar.component.html',
  styleUrls: ['./custom-toolbar.component.scss'],
  animations: [
    trigger('transformSearch', [
      transition(':enter', [
        style({opacity: 0, transform: 'scale(0.1)'}),
        animate('300ms ease-in-out', style({opacity: 1, transform: 'scale(1)'})),
      ]),
      transition(':leave', [
        style({opacity: 1, transform: 'scale(1)'}),
        animate('0ms ease-in-out', style({opacity: 0, transform: 'scale(0.1) translateX(100%)'})),
      ]),
    ]),
  ]
})
export class CustomToolbarComponent implements OnInit {
  @ViewChild('searchbar', {static: false}) searchbar!: IonSearchbar;
  isSearch: boolean = false;
  searchOnGoing: boolean = false;
  searchResults: (AnimalSpeciesSearchFeed | AnimalSearchFeed)[] = [];

  searchBoxForm = this.fb.group({
    searchQuery: ['', Validators.required, Validators.min(4)]
  });

  constructor(public http: HttpClient, public state: State, public router: Router, private readonly token: TokenService, public fb: FormBuilder) {
  }

  ngOnInit() {
    this.getSpecies();
  }

  toggleSearch() {
    this.isSearch = !this.isSearch;
    this.clearSearchInput();

    if (this.isSearch) {
      setTimeout(() => {
        this.searchbar.setFocus();
      }, 100)
    }
  }

  clearSearchInput() {
    this.searchBoxForm.get('searchQuery')!.setValue('');
  }

  async search(event: any) {
    if (event && event.target) {
      const searchQuery = event.target.value;
      if (searchQuery.length >= 4) {
        const response = await firstValueFrom(this.http.get<SearchFeed>("http://localhost:5000/api/search?searchterm=" + searchQuery));
        console.log('response: ', response)
        let result: (AnimalSpeciesSearchFeed | AnimalSearchFeed)[] = [];
        if (response.AnimalSpecies) {
          this.searchResults
          console.log(response.AnimalSpecies)
        }
        if (response.Animals) {
          this.searchResults.concat(response.Animals)
          console.log(response.Animals)
        }
        console.log('result: ', result, this.searchResults)
      }
    }
  }

  goToStaff() {
    this.router.navigate(['/staff']);
  }

  goToHome() {
    this.router.navigate(['']);
  }

  async getSpecies() {
    const result = await firstValueFrom(this.http.get<AnimalSpeciesFeed[]>('http://localhost:5000/api/animalspeciesfeed'))
    this.state.animalSpeciesFeed = result!;
  }

  goToSpecies(animalNumber: number) {
    this.state.currentAnimalSpecies.speciesID = animalNumber;
    this.router.navigate(['/species/' + animalNumber])
  }

  protected readonly focus = focus;

  logOut() {
    this.token.clearToken();
    this.router.navigateByUrl("/login");
  }
}
