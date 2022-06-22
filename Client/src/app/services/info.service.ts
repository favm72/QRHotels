import { Injectable } from "@angular/core"
import {
	INFO_BREAKFAST_SCHED,
	INFO_DIRECTORY,
	INFO_HOTELCODE,
	INFO_HOTELS,
	INFO_HOTELINFO,
	INFO_LATECHECK,
	INFO_PAYMENT_TYPES,
	INFO_PROVIDERS,
	INFO_SLIDER,
} from "../api/endpoints"
import { ApiService, MyReq } from "./api.service"

@Injectable({
	providedIn: "root",
})
export class InfoService {
	constructor(private api: ApiService) {}

	async loadDirectory() {
		let directory = []
		let req = new MyReq("load directory")
		req.url = INFO_DIRECTORY
		req.success = res => {
			directory = res.data
		}
		await this.api.request(req)
		return directory
	}

	async loadSchedule() {
		let schedule = []
		let req = new MyReq("load schedule")
		req.url = INFO_BREAKFAST_SCHED
		req.success = res => {
			schedule = res.data
		}
		await this.api.request(req)
		return schedule
	}

	async hotelCode() {
		let hotelCode = null
		let req = new MyReq("load hotel code")
		req.url = INFO_HOTELCODE
		req.success = res => {
			hotelCode = res.data
		}
		await this.api.request(req)
		return hotelCode
	}

	async loadSlider() {
		let slider = []
		let req = new MyReq("load slider")
		req.url = INFO_SLIDER
		req.success = res => {
			slider = res.data
		}
		await this.api.request(req)
		return slider
	}

	async loadLateCheckout() {
		let lateCheckout = []
		let req = new MyReq("load latecheck services")
		req.url = INFO_LATECHECK
		req.success = res => {
			lateCheckout = res.data
		}
		await this.api.request(req)
		return lateCheckout
	}

	async loadHotels() {
		let hotels = []
		let req = new MyReq("load hotels")
		req.url = INFO_HOTELS
		req.isAdmin = true
		req.success = res => {
			hotels = res.data
		}
		await this.api.request(req)
		return hotels
	}

	async loadHotelInfo() {
		let hotel = null
		let req = new MyReq("load hotel info")
		req.url = INFO_HOTELINFO
		req.success = res => {
			hotel = res.data
		}
		await this.api.request(req)
		return hotel
	}

	async paymtypes() {
		let paymtypes = []
		let req = new MyReq("load payment types")
		req.url = INFO_PAYMENT_TYPES
		req.success = res => {
			paymtypes = res.data
		}
		await this.api.request(req)
		return paymtypes
	}

	async loadProviders() {
		let providers = []
		let req = new MyReq("load providers")
		req.url = INFO_PROVIDERS
		req.isAdmin = true
		req.success = res => {
			providers = res.data
		}
		await this.api.request(req)
		return providers
	}
}
