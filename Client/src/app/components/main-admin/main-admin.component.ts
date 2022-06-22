import { Component, OnDestroy, OnInit, Inject } from "@angular/core"
import { Router } from "@angular/router"
import { OrderService } from "src/app/services/order.service"
import { faSearch } from "@fortawesome/free-solid-svg-icons"
import { SignalOrderService } from "src/app/services/signal-order.service"
import { Subscription } from "rxjs"
import { InfoService } from "src/app/services/info.service"
import { IDropdownSettings } from "ng-multiselect-dropdown"
import { HostListener } from "@angular/core"
// import { Document } from '@angular/platform-browser';

@Component({
	selector: "app-main-admin",
	templateUrl: "./main-admin.component.html",
	styleUrls: ["./main-admin.component.css"],
})
export class MainAdminComponent implements OnInit, OnDestroy {
	icon = faSearch
	filters = null
	orders = []
	hotels = []
	providers = []
	signalSub: Subscription = null
	dropdownList = []
	selectedItems = []
	dropdownSettings: IDropdownSettings = {}
	interval = null
	mounted = false

	constructor(
		private router: Router,
		private orderService: OrderService,
		private info: InfoService,
		private signal: SignalOrderService // @Inject(Document) document: any
	) {
		this.filters = orderService.filters
	}

	ngOnDestroy(): void {
		this.mounted = false
		console.log("ngOnDestroy", this.mounted)
		if (this.signalSub != null) {
			this.signalSub.unsubscribe()
		}
		if (this.interval) clearInterval(this.interval)
	}

	async ngOnInit(): Promise<void> {
		this.mounted = true
		console.log("ngOnInit", this.mounted)
		this.hotels = await this.info.loadHotels()
		this.providers = await this.info.loadProviders()
		// this.providers.forEach(x=>this.filters.idProvider.push({id:x.id,description:x.description}))

		this.selectedItems = [
			// { id: 3, item_text: 'Pune' },
			// { id: 4, item_text: 'Navsari' }
		]
		this.dropdownSettings = {
			singleSelection: false,
			idField: "id",
			textField: "description",
			selectAllText: "Todos",
			unSelectAllText: "UnSelect All",
			itemsShowLimit: 3,
			allowSearchFilter: false,
		}

		await this.loadOrders()
		this.startInterval()
	}

	startInterval(): void {
		console.log("startInterval", this.mounted)
		if (this.mounted) {
			this.interval = setInterval(() => {
				this.loadOrders()
			}, 10000)
		}
	}

	onItemSelect(item: any) {
		console.log(item)
		// this.selectedItems.forEach(x=>console.log(x.id));
	}
	onSelectAll(items: any) {
		console.log(items)
	}

	@HostListener("window:focus", ["$event"])
	onFocus(event: FocusEvent): void {
		console.log("focus", this.mounted)
		if (this.interval == null && this.mounted) {
			this.startInterval()
		}
	}

	// @HostListener("window:blur", ["$event"])
	// onBlur(event: FocusEvent): void {
	// 	console.log("abandonó la pestaña")
	// }

	async notificacionSonido() {
		const audio = new Audio("../../../assets/sound/notification.mp3")
		audio.play()
	}

	async updateFilters() {
		this.orderService.filters.firstName = this.filters.firstName
		this.orderService.filters.lastName = this.filters.lastName
		this.orderService.filters.roomCode = this.filters.roomCode
		this.orderService.filters.statusCode = this.filters.statusCode
		this.orderService.filters.hotelCode = this.filters.hotelCode
		this.orderService.filters.idProvider = this.filters.idProvider
		await this.loadOrders()
	}

	async loadOrders(): Promise<void> {
		await this.orderService.getOrders()
		this.orders = this.orderService.orders
		var ordersPendientes = this.orders.filter(x => x.statusCode == "P")
		if (ordersPendientes != null && ordersPendientes.length > 0) {
			this.notificacionSonido()
		}
	}

	async next(): Promise<void> {
		this.orderService.filters.currentPage++
		this.filters.currentPage = this.orderService.filters.currentPage
		this.loadOrders()
	}

	async prev(): Promise<void> {
		if (this.orderService.filters.currentPage > 1) {
			this.orderService.filters.currentPage--
			this.filters.currentPage = this.orderService.filters.currentPage
			this.loadOrders()
		}
	}
}
