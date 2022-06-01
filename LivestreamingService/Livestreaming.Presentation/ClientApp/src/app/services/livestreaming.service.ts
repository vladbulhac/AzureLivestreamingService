import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LivestreamEndpoints } from '../dtos/livestreamendpoints.dto';
import { LivestreamFullEndpoints } from '../dtos/livestreamfullendpoints.dto';
import { LivestreamsHistoryDTO } from '../dtos/livestreamshistory.dto';
import { LivestreamDetails } from '../dtos/livestreamdetails.dto';
import { SetupLivestream } from '../dtos/setuplivestream.dto';
import { SetupLivestreamResult } from '../dtos/setuplivestreamresult.dto';
import { StartLivestream } from '../dtos/startlivestream.dto';
import { StopLivestream } from '../dtos/stoplivestream.dto';

@Injectable({
  providedIn: 'root'
})
export class LivestreamingService {
  private httpClient: HttpClient;
  private readonly baseAPIroute: string = "api/v1/livestreams";
  private readonly baseUrl: string = "https://localhost:7057/"

  constructor(httpClient: HttpClient) {
    this.httpClient = httpClient;
  }

  public getLivestreams(userId: string, page: number) {
    return this.httpClient.get<LivestreamsHistoryDTO>(this.baseUrl + this.baseAPIroute + "/ofUser/" + userId + "/" + page, { observe: 'response' });
  }

  public getLivestreamEndpoints(livestreamId: string): Observable<LivestreamFullEndpoints> {
    const url = this.baseUrl + this.baseAPIroute + "/" + livestreamId + "/watch";
    return this.httpClient.get<LivestreamFullEndpoints>(url);
  }

  public setupLivestream(setupData: SetupLivestream): Observable<SetupLivestreamResult> {
    return this.httpClient.post<SetupLivestreamResult>(this.baseUrl + this.baseAPIroute + "/setup", setupData);
  }

  public getLivestreamStatus(livestreamId: string): Observable<LivestreamDetails> {
    const url = this.baseUrl + this.baseAPIroute + "/" + livestreamId + "/status";
    return this.httpClient.get<LivestreamDetails>(url);
  }

  public startLivestream(startData: StartLivestream): Observable<LivestreamEndpoints> {
    const url = this.baseUrl + this.baseAPIroute + "/" + startData.livestreamId + "/start";
    return this.httpClient.post<LivestreamEndpoints>(url, { "userId": startData.userId });
  }

  public stopLivestream(stopData: StopLivestream) {
    const url = this.baseUrl + this.baseAPIroute + "/" + stopData.livestreamId + "/stop";
    return this.httpClient.post(url, { "userId": stopData.userId });
  }
}
