import { Component, OnInit } from "@angular/core"
import { DirectoryService } from "src/app/services/directory.service"
import { InfoService } from "src/app/services/info.service"
import { NotificationService } from "src/app/services/notification.service"
import { faArrowLeft } from "@fortawesome/free-solid-svg-icons"

@Component({
	selector: "app-directory",
	templateUrl: "./directory.component.html",
	styleUrls: ["./directory.component.css"],
})
export class DirectoryComponent implements OnInit {
	backIcon = faArrowLeft
	head: any = null
	detail = []
	currentItem = null
	constructor(private info: InfoService, private notif: NotificationService, private directory: DirectoryService) {}

	async ngOnInit(): Promise<void> {
		const data = await this.directory.list()
		console.log(data)
		this.head = data.head ?? null
		this.detail = data.detail ?? []
	}

	viewItem(item) {
		this.currentItem = item
	}

	goBack() {
		this.currentItem = null
	}

	decode(str) {
		return decodeURIComponent(atob(str))
	}
	// toggle(index) {
	// 	this.directory[index].active = !this.directory[index].active
	// }
}
