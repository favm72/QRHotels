import { Injectable } from "@angular/core"
import { ANALYTICS_LOG } from "../api/endpoints"
import { ApiService, MyReq } from "./api.service"

@Injectable({
	providedIn: "root",
})
export class AnalyticsService {
	constructor(private api: ApiService) {}

	async log(action: string) {
		let req = new MyReq("log " + action)
		req.url = ANALYTICS_LOG
		req.model = { action: action }
		req.success = _ => {}
		req.notify = false
		await this.api.request(req)
	}
}
