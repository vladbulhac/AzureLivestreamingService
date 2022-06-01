import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { LivestreamsHistoryDTO } from '../dtos/livestreamshistory.dto';
import { LivestreamDetails } from '../dtos/livestreamdetails.dto';
import { LivestreamingService } from '../services/livestreaming.service';

@Component({
  selector: 'app-livestream-history',
  templateUrl: './livestream-history.component.html',
  styleUrls: ['./livestream-history.component.css']
})
export class LivestreamHistoryComponent implements OnInit {
  public messageContent: string;
  public livestreams: LivestreamDetails[];
  private livestreamService: LivestreamingService;

  private route: ActivatedRoute;
  private router: Router;
  private userId: string | null;
  private page: number;
  constructor(route: ActivatedRoute, router: Router, livestreamService: LivestreamingService) {
    this.livestreamService = livestreamService;
    this.livestreams = [];
    this.route = route;
    this.router = router;
    this.userId = null;
    this.page = 1;
    this.messageContent = "";
  }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      console.log(params);
      this.page = params['page'];
      this.userId = params['userId'];
    });

    this.livestreamService.getLivestreams(this.userId!, this.page).subscribe((result) => {
      this.livestreams = [];
      for (let livestream of result.body!.livestreams) {
        this.livestreams.push(livestream as LivestreamDetails);
        console.log(livestream);
      }
    }, (error) => {
      console.log(error);
      this.messageContent = `<p>Could not fetch this user's livestreams.</p>`;
    });
  }

  redirectToStatusPage(id: string) {
    this.router.navigate(['/livestreams', id, 'status']);
  }

  watchVideo(id: string) {
    this.router.navigate(['/livestreams', id, 'watch']);
  }
}
