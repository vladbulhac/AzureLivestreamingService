import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-livestream-howto',
  templateUrl: './livestream-howto.component.html',
  styleUrls: ['./livestream-howto.component.css']
})
export class LivestreamHowtoComponent implements OnInit {
  panelOpenState = false;

  @Input() ingestUrl!: string;
  constructor() { }

  ngOnInit(): void {
  }
}
