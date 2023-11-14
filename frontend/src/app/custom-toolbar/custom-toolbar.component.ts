import {Component, OnInit} from '@angular/core';
import {animate, style, transition, trigger} from "@angular/animations";

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
  isSearch: boolean = false;
  constructor() { }

  ngOnInit() {}

  toggleSearch() {
    this.isSearch = !this.isSearch;
  }
}
