import { Injectable } from "@angular/core"
import { Subject } from "rxjs"
import { AlertModel } from "../models/notification"

@Injectable({
	providedIn: "root",
})
export class NotificationService {
	onAlert: Subject<AlertModel> = new Subject<AlertModel>()
	onLoading: Subject<boolean> = new Subject<boolean>()
	onUpdateDetail: Subject<void> = new Subject<void>()

	showLoading() {
		this.onLoading.next(true)
	}

	hideLoading() {
		this.onLoading.next(false)
	}

	info(message: string) {
		let am = new AlertModel()
		am.message = message
		this.onAlert.next(am)
	}

	success(message: string) {
		let am = new AlertModel()
		am.message = message
		am.type = "success"
		this.onAlert.next(am)
	}

	error(message: string) {
		let am = new AlertModel()
		am.message = message
		am.type = "danger"
		this.onAlert.next(am)
	}

	newOrder(provider: number) {
		let am = new AlertModel()
		am.type = "success"
		am.options.newOrder = true
		am.options.provider = provider
		this.onAlert.next(am)
	}

	constructor() {}
}
