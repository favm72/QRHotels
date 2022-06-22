import { Component, OnDestroy, OnInit } from "@angular/core"
import { Router } from "@angular/router"
import { faBars } from "@fortawesome/free-solid-svg-icons"
import { faBell } from "@fortawesome/free-solid-svg-icons"
import { Subscription } from "rxjs"
import { AccountService } from "src/app/services/account.service"
import { InfoService } from "src/app/services/info.service"
import { NotificationService } from "src/app/services/notification.service"
import { OrderService } from "src/app/services/order.service"
import { SignalOrderService } from "src/app/services/signal-order.service"

@Component({
	selector: "app-master",
	templateUrl: "./master.component.html",
	styleUrls: ["./master.component.css"],
})
export class MasterComponent implements OnInit, OnDestroy {
	menuicon = faBars
	notificon = faBell
	fullname: string = ""
	roomcode: string = ""
	menuActive: boolean = false
	signalSub: Subscription = null
	markSub: Subscription = null
	hotel: any = {}
	notifications = []
	//notifActive : boolean = false

	showMenu() {
		//this.notifActive = false
		this.menuActive = !this.menuActive
	}

	onLogo() {
		this.router.navigate(["/master/main"])
	}

	async showNotifications(): Promise<void> {
		this.menuActive = false
		if (this.notifications.length == 0) {
			this.notif.info("No tiene notificaciones.")
			return
		}
		let lastElement = this.notifications[this.notifications.length - 1]
		await this.order.setSelectedOrder(lastElement.id, false)
		if (this.order.selectedOrder == null) {
			this.notif.info(`No se encontró al orden con id ${lastElement.id}`)
			return
		}
		if (this.router.url == "/master/detail") {
			this.notif.onUpdateDetail.next()
		} else {
			this.router.navigate(["/master/detail"])
		}
	}

	constructor(
		private router: Router,
		private signalServ: SignalOrderService,
		private order: OrderService,
		private notif: NotificationService,
		private account: AccountService,
		private info: InfoService
	) {}

	ngOnDestroy(): void {
		if (this.signalSub != null) {
			this.signalSub.unsubscribe()
		}
		if (this.markSub != null) {
			this.markSub.unsubscribe()
		}
	}

	logout() {
		localStorage.removeItem("qrh")
		this.router.navigate(["/"])
	}

	async getHotel() {
		this.hotel = await this.info.loadHotelInfo()
	}

	async ngOnInit(): Promise<void> {
		if (localStorage.qrh == null) {
			this.router.navigate(["/login"])
			return
		}
		this.getHotel()
		let user = JSON.parse(localStorage.qrh)
		this.fullname = user.fullname
		this.roomcode = user.roomCode

		this.notifications = await this.order.notifByClient()

		this.signalSub = this.signalServ.onGetOrder.subscribe(data => {
			this.notifications.push(data)
		})

		this.markSub = this.order.onViewNotifClient.subscribe(async () => {
			this.notifications = await this.order.notifByClient()
		})
	}
}
