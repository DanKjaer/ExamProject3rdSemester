import {Component, OnInit, ViewChild} from '@angular/core';
import {animate, style, transition, trigger} from "@angular/animations";
import {IonSearchbar} from "@ionic/angular";
import {Router} from "@angular/router";

@Component({
  selector: 'app-custom-toolbar',
  templateUrl: './custom-toolbar.component.html',
  styleUrls: ['./custom-toolbar.component.scss'],
  animations: [
    trigger('transformSearch', [
      transition(':enter', [
        style({ opacity: 0, transform: 'scale(0.1)'}),
        animate('300ms ease-in-out', style({ opacity: 1, transform: 'scale(1)' })),
      ]),
      transition(':leave', [
        style({ opacity: 1, transform: 'scale(1)' }),
        animate('0ms ease-in-out', style({ opacity: 0, transform: 'scale(0.1) translateX(100%)' })),
      ]),
    ]),
  ]
})
export class CustomToolbarComponent  implements OnInit {
  @ViewChild('searchbar', {static: false}) searchbar!: IonSearchbar;
  isSearch: boolean = false;
  constructor(public router: Router) { }

  ngOnInit() {}

  toggleSearch() {
    this.isSearch = !this.isSearch;

    if (this.isSearch) {
      setTimeout(() => {
        this.searchbar.setFocus();
      }, 100)
    }
  }

  goToProfile(){
    this.router.navigate(['/profile'])
  }

  protected readonly focus = focus;
}
